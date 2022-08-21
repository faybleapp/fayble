import ComicVine from "assets/logos/providers/ComicVine.png";
import { Image } from "components/image";
import { LoadingIndicator } from "components/loadingIndicator";
import { Button, Container, Form, InputGroup } from "react-bootstrap";
import { useSeriesMetadata } from "services";
import styles from "./SeriesMatch.module.scss";

interface SeriesMatchProps {
  seriesId: string;
  unMatchSeries: () => void;
}

export const SeriesMatch = ({ seriesId, unMatchSeries }: SeriesMatchProps) => {
  const { data: matchedSeries, isLoading } = useSeriesMetadata(seriesId);

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
              <InputGroup.Text className={styles.label}>
                Series
              </InputGroup.Text>
              <Form.Control
                disabled
                value={matchedSeries?.name}
              />
            </InputGroup>
            <InputGroup className="mb-4">
              <InputGroup.Text className={styles.label}>
                Fayble ID
              </InputGroup.Text>
              <Form.Control
                disabled               
                value={matchedSeries?.id}
              />
            </InputGroup>
            <InputGroup className="mb-4">
              <InputGroup.Text               
                className={styles.label}>
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
            <Button
              className={styles.unmatchButton}
              variant="primary"
              size="sm"
              onClick={unMatchSeries}>
              Unmatch
            </Button>
          </div>
        </div>
      )}
    </Container>
  );
};
