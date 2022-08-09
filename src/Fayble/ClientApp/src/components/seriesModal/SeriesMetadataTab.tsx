import { Series } from "models/api-models";
import { useState } from "react";
import { Container } from "react-bootstrap";
import { SeriesMatch } from "./SeriesMatch";
import { SeriesSearch } from "./SeriesSearch";
interface SeriesMetadataTabProps {
  series: Series;
}

export const SeriesMetadataTab = ({ series }: SeriesMetadataTabProps) => {
  const [matchedSeriesId, setMatchedSeriesId] = useState<string | undefined>();

  return (
    <Container>
      {matchedSeriesId ? (
        <SeriesMatch
          seriesId={matchedSeriesId}
          unMatchSeries={() => setMatchedSeriesId(undefined)}
        />
      ) : (
        <SeriesSearch series={series} onMatchedSeries={setMatchedSeriesId} />
      )}
    </Container>
  );
};
