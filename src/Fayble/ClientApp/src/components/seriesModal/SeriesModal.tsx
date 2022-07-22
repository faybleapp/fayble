import { yupResolver } from "@hookform/resolvers/yup";
import { Form } from "components/form";
import { ModalTabs } from "components/modalTabs";
import { Series } from "models/api-models";
import { Button, Container, Modal, Spinner, Tab } from "react-bootstrap";
import { SubmitHandler, useForm } from "react-hook-form";
import { useUpdateSeries } from "services";
import * as yup from "yup";
import { SeriesDetailsTab } from "./SeriesDetailsTab";
import { SeriesMetadataTab } from "./SeriesMetadataTab";
import styles from "./SeriesModal.module.scss";

interface SeriesModalProps {
  series: Series;
  show: boolean;
  close: () => void;
}

export const SeriesModal = ({ series, show, close }: SeriesModalProps) => {
  const updateSeries = useUpdateSeries();

  const validationSchema = yup.object().shape({
    name: yup.string().trim().required("Required"),
    volume: yup.string().trim(),
    year: yup
      .number()
      .positive()
      .nullable(true)
      .min(1000, "Invalid year")
      .max(9999, "Invalid year"),
  });

  const form = useForm<Series>({
    defaultValues: series,
    resolver: yupResolver(validationSchema),
  });

  const handleSubmit: SubmitHandler<Series> = (values) => {
    updateSeries.mutate([values.id, values], {
      onSuccess: () => {
        close();
      },
    });
  };

  return (
    <Modal size="lg" show={show} onHide={close}>
      <Modal.Header closeButton>
        <Container>
          <Modal.Title>Edit Series</Modal.Title>
        </Container>
      </Modal.Header>
      <Modal.Body>
        <Form<Series> form={form}>
          <ModalTabs defaultActiveKey="details">
            <Tab eventKey="details" title="Details">
              <SeriesDetailsTab />
            </Tab>
            <Tab eventKey="metadata" title="Metadata">
              <SeriesMetadataTab series={series} />
            </Tab>
            <Tab eventKey="settings" title="Settings"></Tab>
          </ModalTabs>
        </Form>
      </Modal.Body>

      <Modal.Footer>
        <Button variant="secondary" onClick={close}>
          Close
        </Button>
        <Button
          variant="primary"
          onClick={form.handleSubmit(handleSubmit)}
          disabled={updateSeries.isLoading || !form.formState.isDirty}>
          {updateSeries.isLoading ? (
            <>
              <Spinner
                className={styles.spinner}
                as="span"
                animation="border"
                size="sm"
                role="status"
                aria-hidden="true"
              />
              Saving...
            </>
          ) : (
            "Save Changes"
          )}
        </Button>
      </Modal.Footer>
    </Modal>
  );
};
