import { Series, SeriesSearchResult } from "models/api-models";
import { useState } from "react";
import { Button, Col, Container, Form, Row, Spinner } from "react-bootstrap";
import { toast } from "react-toastify";
import { useHttpClient } from "services/httpClient";
import { SearchResultItem } from "./SearchResultItem";

interface SeriesMetadataTabProps {
  series: Series;
}

export const SeriesMetadataTab = ({ series }: SeriesMetadataTabProps) => {
  const client = useHttpClient();
  const [name, setName] = useState<string>(series.name || "");
  const [year, setYear] = useState<string>(series.year?.toString() || "");
  const [searchResults, setSearchResults] = useState<SeriesSearchResult[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const handleCLick = async () => {
    setIsLoading(true);
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

  return (
    <Container>
      <Row>
        <Col xs={9}>
          <Form.Control
            name="name"
            value={name}
            placeholder="name"
            onChange={(e) => setName(e.target.value)}
          />
        </Col>
        <Col xs={3}>
          <Form.Control
            type="number"
            name="year"
            value={year}
            placeholder="year"
            onChange={(e) => setYear(e.target.value)}
          />
        </Col>
      </Row>
      <Button
        disabled={!name.trim() || isLoading}
        onClick={handleCLick}
        size="sm"
        style={{ width: "100%" }}>
        {isLoading ? (
          <>
            <Spinner
              as="span"
              animation="border"
              size="sm"
              role="status"
              aria-hidden="true"
            />
            {"  Searching..."}
          </>
        ) : (
          "Search"
        )}
      </Button>

      {searchResults.map((result) => (
        <SearchResultItem searchResult={result} />
      ))}
    </Container>
  );
};
