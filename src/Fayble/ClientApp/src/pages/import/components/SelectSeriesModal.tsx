import { SelectField } from "components/selectField";
import { Container, Modal } from "react-bootstrap";

import { useAllSeries } from "services";
interface SelectSeriesModalProps {
  show: boolean;
  onClose: () => void;
  onSelectSeries: (id: string) => void;
}

export const SelectSeriesModal = ({
  show,
  onClose,
  onSelectSeries,
}: SelectSeriesModalProps) => {
  const { data: series } = useAllSeries();

  return (
    <Modal show={show} onHide={onClose}>
      <Modal.Header closeButton>
        <Container>
          <Modal.Title>Select Series</Modal.Title>
        </Container>
      </Modal.Header>
      <Modal.Body>
        <Container>
          {series && (
            <SelectField
              onChange={(seriesId) => onSelectSeries(seriesId)}
              name="series"
              options={
                series.map((seriesItem) => ({
                  value: seriesItem.id!,
                  label: seriesItem.name!,
                })) || []
              }
            />
          )}
        </Container>
      </Modal.Body>
      <Modal.Footer></Modal.Footer>
    </Modal>
  );
}; 