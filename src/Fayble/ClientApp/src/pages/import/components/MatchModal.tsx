import { LoadingIndicator } from "components/loadingIndicator";
import { SelectField } from "components/selectField";
import { Container, Modal } from "react-bootstrap";
import { useSeriesMetadata } from "services";
import styles from "./MatchModal.module.scss";

interface MatchModalProps {
  show: boolean;
  seriesMatchId: string;
  matchId: string;
  filename?: string;
  onClose: () => void;
  onMatch: (id: string) => void;
}

export const MatchModal = ({
  show,
  seriesMatchId,
  matchId,
  filename,
  onClose,
  onMatch,
}: MatchModalProps) => {
  const { data: seriesMetadata, isLoading: isLoadingMetadata } =
    useSeriesMetadata(seriesMatchId);  
  return (
    <Modal show={show} onHide={onClose}>
      <Modal.Header closeButton>
        <Container>
          <Modal.Title>Match</Modal.Title>
        </Container>
      </Modal.Header>
      <Modal.Body>
        <Container>
          {isLoadingMetadata ? (
            <div className={styles.loadingIndicator}>
              <LoadingIndicator />
            </div>
          ) : (
            <>
              <SelectField
                onChange={(seriesId) => onMatch(seriesId)}
                value={matchId || ""}
                clearable
                name="series"
                options={
                  seriesMetadata?.books?.map((book) => ({
                    value: book.id,
                    label: `${book.number.padStart(3, "0")}${
                      book.title && " - " + book.title
                    }`,
                  })) || []
                }
              />

              <div className={styles.filename}>{filename}</div>
            </>
          )}
        </Container>
      </Modal.Body>
      <Modal.Footer>       
      </Modal.Footer>
    </Modal>
  );
};
