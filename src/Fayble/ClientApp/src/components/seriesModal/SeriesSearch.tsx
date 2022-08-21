import { Series, SeriesSearchResult } from "models/api-models";
import { useEffect, useState } from "react";
import { Button, Form, InputGroup, Spinner } from "react-bootstrap";
import { toast } from "react-toastify";
import { useHttpClient } from "services/httpClient";
import { SearchResultItem } from "./SearchResultItem";
import styles from "./SeriesSearch.module.scss";

interface SeriesSearchProps {
  series: Series;
  onMatchedSeries: (seriesId: string | undefined) => void;
}

export const SeriesSearch = ({
  series,
  onMatchedSeries,
}: SeriesSearchProps) => {
  const client = useHttpClient();

  const [searchQuery, setSearchQuery] = useState<string>("");
  const [searchResults, setSearchResults] = useState<SeriesSearchResult[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  useEffect(() => {
    if (!series.name) return;
    const yearQuery = series.year ? ` YEAR:${series.year}` : "";
    setSearchQuery(series.name + yearQuery);
  }, [series]);

  const handleSearch = async () => {
    setIsLoading(true);
    setSearchResults([]);
    await client
      .get<SeriesSearchResult[]>(
        `/metadata/searchseries?searchQuery=${searchQuery}`
      )
      .then((response) => {
        setSearchResults(response.data);
      })
      .catch(() => {
        setSearchResults([]);
        toast.error("An error occured while searching series");
      })
      .finally(() => {
        setIsLoading(false);
      });
  };

  const handleSelect = (searchResultId: string) => {
    setSearchResults([]);
    onMatchedSeries(
      searchResults.find((result) => result.id === searchResultId)?.id
    );
  };

  return (
    <div>
      <InputGroup className={styles.container}>
        <Form.Control
          name="searchQuery"
          onKeyPress={(event: React.KeyboardEvent) => {
            if (event.key === 'Enter') {
              event.preventDefault();
              handleSearch();

            }
          }}
          value={searchQuery}
          disabled={isLoading}
          placeholder="name"
          onChange={(e) => setSearchQuery(e.target.value)}
        />
        <Button
          disabled={!searchQuery.trim() || isLoading}
          style={{ width: "75px" }}
          onClick={handleSearch}>
          {isLoading ? (
            <Spinner
              as="span"
              animation="border"
              size="sm"
              role="status"
              aria-hidden="true"
            />
          ) : (
            "Search"
          )}
        </Button>
      </InputGroup>
      <div className={styles.searchResults}>
        {searchResults.map((result) => (
          <SearchResultItem searchResult={result} onSelect={handleSelect} />
        ))}
      </div>
    </div>
  );
};
