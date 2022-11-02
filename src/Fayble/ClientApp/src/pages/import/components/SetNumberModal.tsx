import { useEffect, useState } from "react";
import { Button, Container, Modal } from "react-bootstrap";
import styles from "./SetNumberModal.module.scss";

interface SetNumberModalProps {
  show: boolean;
  number: string;
  onClose: (number: string) => void;  
}

export const SetNumberModal = ({
  show,
  number,
  onClose,  
}: SetNumberModalProps) => {
  const [newNumber, setNewNumber] = useState<string>(number);
  useEffect(() => {
    setNewNumber(number);
  }, [number]);

  return (
    <Modal show={show} onHide={() => onClose(newNumber)}>
      <Modal.Header closeButton>
        <Container>
          <Modal.Title>Number</Modal.Title>
        </Container>
      </Modal.Header>
      <Modal.Body>
        <Container>
          <input
            value={newNumber}
            className={styles.numberField}
            onChange={(e) => setNewNumber(e.target.value)}
          />
        </Container>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={() => onClose(newNumber)}>
          Close
        </Button>
      </Modal.Footer>
    </Modal>
  );
};
