import { Container, Modal } from "react-bootstrap";
import styles from "./SetImportFilenameModal.module.scss";

interface SetImportFilenameModalProps {
  show: boolean;
  filename: string;
  onClose: () => void;
  onChange: (id: string) => void;
}

export const SetImportFilenameModal = ({
  show,
  filename,
  onClose,
  onChange,
}: SetImportFilenameModalProps) => {
  return (
    <Modal show={show} onHide={onClose}>
      <Modal.Header closeButton>
        <Container>
          <Modal.Title>Filename</Modal.Title>
        </Container>
      </Modal.Header>
      <Modal.Body>
        <Container>
          <input
            value={filename}
            className={styles.numberField}
            onChange={(e) => onChange(e.target.value)}
          />
        </Container>
      </Modal.Body>
      <Modal.Footer></Modal.Footer>
    </Modal>
  );
};
