import { Container, Modal } from "react-bootstrap";
import styles from "./SetNumberModal.module.scss";

interface SetNumberModalProps {
  show: boolean;
  number: string;
  onClose: () => void;
  onChange: (id: string) => void;
}

export const SetNumberModal = ({
  show,
  number,
  onClose,
  onChange,
}: SetNumberModalProps) => {
  return (
    <Modal show={show} onHide={onClose}>
      <Modal.Header closeButton>
        <Container>
          <Modal.Title>Number</Modal.Title>
        </Container>
      </Modal.Header>
      <Modal.Body>
        <Container>
          <input
            value={number}
            className={styles.numberField}
            onChange={(e) => onChange(e.target.value)}
          />
        </Container>
      </Modal.Body>
      <Modal.Footer></Modal.Footer>
    </Modal>
  );
};
