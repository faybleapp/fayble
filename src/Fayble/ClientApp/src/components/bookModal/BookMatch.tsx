import ComicVine from "assets/logos/providers/ComicVine.png";
import { Image } from "components/image";
import { LoadingButton } from "components/loadingButton";
import { LoadingIndicator } from "components/loadingIndicator";
import { Button, Container, Form, InputGroup } from "react-bootstrap";
import { useBookMetadata } from "services";
import styles from "./BookMatch.module.scss";
interface BookMatchProps {
  bookMatchId: string;
  isRefreshingMetadata: boolean;
  showRefreshMetadata: boolean;
  onUnmatchBook: () => void;
  onRefreshMetadata: () => void;
}

export const BookMatch = ({
  bookMatchId,
  onUnmatchBook,
  onRefreshMetadata,
  isRefreshingMetadata,
  showRefreshMetadata,
}: BookMatchProps) => {
  const { data: matchedBook, isLoading } = useBookMetadata(bookMatchId);
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
              src={matchedBook?.image || ""}></Image>
          </div>
          <div className={styles.detailsContainer}>
            <InputGroup className="mb-4">
              <InputGroup.Text className={styles.label}>Book</InputGroup.Text>
              <Form.Control disabled value={matchedBook?.title} />
            </InputGroup>
            <InputGroup className="mb-4">
              <InputGroup.Text className={styles.label}>
                Fayble ID
              </InputGroup.Text>
              <Form.Control disabled value={matchedBook?.id} />
            </InputGroup>
            <InputGroup className="mb-4">
              <InputGroup.Text className={styles.label}>
                <img src={ComicVine} style={{ width: "43px" }} />
              </InputGroup.Text>
              <Form.Control
                disabled
                value={
                  matchedBook?.providers?.find(
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
                onClick={onUnmatchBook}>
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
