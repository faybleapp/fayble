import { Button, Modal } from "react-bootstrap";

interface SelectSeriesModalProps {
  show: boolean;
  onClose: () => void;
  onSelectSeries: (id: string) => void;
}

export const SelectSeriesModal = ({ show, onClose, onSelectSeries }: SelectSeriesModalProps) => {
  return (
    <Modal show={show} onHide={onClose}>
      <Modal.Header closeButton>
        <Modal.Title>Modal heading</Modal.Title>
      </Modal.Header>
      <Modal.Body>Woohoo, you're reading this text in a modal!</Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onClose}>
          Close
        </Button>
        <Button variant="primary" onClick={() => onSelectSeries("BLASRG")}>
          Save Changes
        </Button>
      </Modal.Footer>
    </Modal>
  );
};
