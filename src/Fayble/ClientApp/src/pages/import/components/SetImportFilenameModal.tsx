import { useEffect, useState } from "react";
import { Button, Container, Modal } from "react-bootstrap";
import styles from "./SetImportFilenameModal.module.scss";

interface SetImportFilenameModalProps {
  show: boolean;
  fileName: string;
  onClose: (number: string) => void;
}

export const SetImportFilenameModal = ({
  show,
  fileName,
  onClose,
}: SetImportFilenameModalProps) => {
  const [newFileName, setNewFileName] = useState<string>(fileName);

  useEffect(() => {
    setNewFileName(fileName);
  }, [fileName]);
  return (
    <Modal show={show} onHide={() => onClose(newFileName)}>
      <Modal.Header closeButton>
        <Container>
          <Modal.Title>Filename</Modal.Title>
        </Container>
      </Modal.Header>
      <Modal.Body>
        <Container>
          <input
            value={newFileName}
            className={styles.numberField}
            onChange={(e) => setNewFileName(e.target.value)}
          />
        </Container>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={() => onClose(newFileName)}>
          Close
        </Button>
      </Modal.Footer>
    </Modal>
  );
};
