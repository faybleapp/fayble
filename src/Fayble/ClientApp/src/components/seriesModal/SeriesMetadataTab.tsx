import { Series } from "models/api-models";
import { Container } from "react-bootstrap";
import { useFormContext, useWatch } from "react-hook-form";
import { toast } from "react-toastify";
import { useRefreshMetadata } from "services";
import { SeriesMatch } from "./SeriesMatch";
import { SeriesSearch } from "./SeriesSearch";
interface SeriesMetadataTabProps {
  series: Series;
}

export const SeriesMetadataTab = ({ series }: SeriesMetadataTabProps) => {
  const { control, setValue, formState } = useFormContext();
  const matchedSeriesId = useWatch({ control, name: "matchId" });
  const { mutate: refreshMetadata, isLoading: isRefreshingMetadata } =
    useRefreshMetadata(series.id);

  const handleRefreshMetadata = () => {
    refreshMetadata(null, {
      onSuccess: () => {
        toast.success(`${series.name} queued for metadata refresh`);
      },
      onError: () => {
        toast.error(
          `An error occurred while queing ${series.name} for metadata refresh`
        );
      },
    });
  };

  const setMatchId = (id?: string) => {
    setValue("matchId", id, { shouldDirty: true });
    
  };

  return (
    <Container>
      {matchedSeriesId ? (
        <SeriesMatch
          isRefreshingMetadata={isRefreshingMetadata}
          showRefreshMetadata={!formState.dirtyFields?.matchId && series.matchId != undefined}
          seriesMatchId={matchedSeriesId}
          onRefreshMetadata={handleRefreshMetadata}
          onUnMatchSeries={() => setMatchId(undefined)}
        />
      ) : (
        <SeriesSearch
          series={series}
          onMatchedSeries={(seriesMatchId) => {
            setMatchId(seriesMatchId);
          }}
        />
      )}
    </Container>
  );
};
