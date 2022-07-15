import { ModalTabs } from "components/modalTabs";
import { useFormik } from "formik";
import { Series } from "models/api-models";
import { Button, Container, Modal, Spinner, Tab } from "react-bootstrap";
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

	const formik = useFormik<Series>({
		initialValues: series!,
		enableReinitialize: true,
		onSubmit: (values: Series) => {
			updateSeries.mutate([values.id, values], {
				onSuccess: () => {
					close();
				},
			});
		},
		validationSchema: yup.object().shape({
			name: yup.string().trim().required("Required"),
			volume: yup.string().trim(),
			year: yup
				.number()
				.positive()
				.nullable(true)
				.min(1000, "Invalid year")
				.max(9999, "Invalid year"),
		}),
		validateOnMount: true,
	});

	return (
		<Modal size="lg" show={show} onHide={close}>
			<Modal.Header closeButton>
				<Container>
					<Modal.Title>Edit Series</Modal.Title>
				</Container>
			</Modal.Header>
			<Modal.Body>
				<ModalTabs defaultActiveKey="details">
					<Tab eventKey="details" title="Details">
						<SeriesDetailsTab formik={formik} />
					</Tab>
					<Tab eventKey="metadata" title="Metadata">
						<SeriesMetadataTab series={series} />
					</Tab>
					<Tab eventKey="settings" title="Settings"></Tab>
				</ModalTabs>
			</Modal.Body>
			<Modal.Footer>
				<Button variant="secondary" onClick={close}>
					Close
				</Button>
				<Button
					variant="primary"
					onClick={formik.submitForm}
					disabled={updateSeries.isLoading || !formik.dirty}>
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
