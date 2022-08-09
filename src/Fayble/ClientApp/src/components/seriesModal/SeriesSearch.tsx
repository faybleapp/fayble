import { Series, SeriesSearchResult } from "models/api-models";
import { useState } from "react";
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

  const [name, setName] = useState<string>(series.name || "");
  const [year, setYear] = useState<string>(series.year?.toString() || "");
  const [searchResults, setSearchResults] = useState<SeriesSearchResult[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const handleSearch = async () => {
    setIsLoading(true);
    setSearchResults([]);
    await client
      .get<SeriesSearchResult[]>(
        `/metadata/searchseries?name=${name}&year=${year}`
      )
      .then((response) => {
        setSearchResults(response.data);
        setIsLoading(false);
      })
      .catch(() => {
        setSearchResults([]);
        toast.error("An error occured while searching series");
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
          name="name"
          value={name}
          disabled={isLoading}
          placeholder="name"
          onChange={(e) => setName(e.target.value)}
        />
        <Button
          disabled={!name.trim() || isLoading}
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
