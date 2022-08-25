import { Series } from "models/api-models";
import { Container } from "react-bootstrap";
import { useFormContext, useWatch } from "react-hook-form";
import { SeriesMatch } from "./SeriesMatch";
import { SeriesSearch } from "./SeriesSearch";
interface SeriesMetadataTabProps {
  series: Series;
}

export const SeriesMetadataTab = ({ series }: SeriesMetadataTabProps) => {
  const { control, setValue } = useFormContext();
  const matchedSeriesId = useWatch({ control, name: "matchId" });

  return (
    <Container>
      {matchedSeriesId ? (
        <SeriesMatch
          seriesId={matchedSeriesId}
          unMatchSeries={() =>
            setValue("matchId", undefined, { shouldDirty: true })
          }
        />
      ) : (
        <SeriesSearch
          series={series}
          onMatchedSeries={(seriesId) => {
            setValue("matchId", seriesId, { shouldDirty: true });
          }}
        />
      )}
    </Container>
  );
};
