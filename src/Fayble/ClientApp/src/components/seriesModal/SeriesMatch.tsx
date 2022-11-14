import ComicVine from "assets/logos/providers/ComicVine.png";
import { Image } from "components/image";
import { LoadingButton } from "components/loadingButton";
import { LoadingIndicator } from "components/loadingIndicator";
import { Button, Container, Form, InputGroup } from "react-bootstrap";
import { useSeriesMetadata } from "services";
import styles from "./SeriesMatch.module.scss";

interface SeriesMatchProps {
  seriesMatchId: string;
  isRefreshingMetadata: boolean;
  showRefreshMetadata: boolean;
  onUnMatchSeries: () => void;
  onRefreshMetadata: () => void;
}

export const SeriesMatch = ({
  seriesMatchId,
  isRefreshingMetadata,
  showRefreshMetadata,
  onUnMatchSeries,
  onRefreshMetadata,
}: SeriesMatchProps) => {
  const { data: matchedSeries, isLoading } = useSeriesMetadata(seriesMatchId);

  return (
    <Container>
      {isLoading ? (
        <div className={styles.loadingIndicator}>
          <LoadingIndicator />
        </div>
      ) : (
        <div className={styles.container}>
          <div className={styles.coverContainer}>
            <Image
              className={styles.cover}
              src={matchedSeries?.image || ""}></Image>
          </div>
          <div className={styles.detailsContainer}>
            <InputGroup className="mb-4">
              <InputGroup.Text className={styles.label}>Series</InputGroup.Text>
              <Form.Control disabled value={matchedSeries?.name} />
            </InputGroup>
            <InputGroup className="mb-4">
              <InputGroup.Text className={styles.label}>
                Fayble ID
              </InputGroup.Text>
              <Form.Control disabled value={matchedSeries?.id} />
            </InputGroup>
            <InputGroup className="mb-4">
              <InputGroup.Text className={styles.label}>
                <img src={ComicVine} style={{ width: "43px" }} />
              </InputGroup.Text>
              <Form.Control
                disabled
                value={
                  matchedSeries?.providers?.find(
                    (provider) => provider.name === "ComicVine"
                  )?.providerItemId
                }
              />
            </InputGroup>
            <div className={styles.buttonContainer}>
              <Button
                className={styles.unmatchButton}
                variant="secondary"
                size="sm"
                onClick={onUnMatchSeries}>
                Unmatch
              </Button>
              {showRefreshMetadata && (
                <LoadingButton
                  isLoading={isRefreshingMetadata}
                  className={styles.refreshButton}
                  variant="primary"
                  text="Refresh Metadata"
                  loadingText="Queuing..."
                  onClick={onRefreshMetadata}
                />
              )}
            </div>
          </div>
        </div>
      )}
    </Container>
  );
};
