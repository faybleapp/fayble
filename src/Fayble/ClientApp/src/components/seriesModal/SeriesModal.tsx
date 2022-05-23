import { NumberField } from "components/form/numberField";
import { SelectField } from "components/form/selectField";
import { TextField } from "components/form/textField";
import { ModalTabs } from "components/modalTabs";
import { useFormik } from "formik";
import { Series } from "models/api-models";
import {
	Button,
	Col,
	Container,
	Form,
	Modal,
	Row,
	Spinner,
	Tab
} from "react-bootstrap";
import { useParams } from "react-router-dom";
import { useLibrarySeries, usePublishers, useUpdateSeries } from "services";
import { TextAreaField } from "textAreaField";
import * as yup from "yup";
import styles from "./SeriesModal.module.scss";

interface SeriesModalProps {
	series: Series;
	show: boolean;
	close: () => void;
}

export const SeriesModal = ({ series, show, close }: SeriesModalProps) => {
	const { libraryId } = useParams<{ libraryId: string }>();

	const updateSeries = useUpdateSeries();

	const publishers = usePublishers();
	const { data: allSeries } = useLibrarySeries(libraryId!);

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
						<Container>
							<Form>
								<TextField
									name="name"
									label="Name"									
									error={
										formik.touched?.name &&
										formik.errors?.name
									}
									value={formik.values.name}
									onChange={formik.handleChange}
								/>
								<Row>
									<Col>
										<NumberField
											name="year"
											label="Year"											
											error={
												formik.touched.year &&
												formik.errors.year
											}
											value={formik.values.year}
											onChange={formik.handleChange}
										/>
									</Col>
									<Col>
										<TextField
											name="volume"
											label="Volume"											
											error={
												formik.touched.volume &&
												formik.errors.volume
											}
											value={formik.values.volume}
											onChange={formik.handleChange}
										/>
									</Col>
								</Row>
								<Row>
									<Col>
										<SelectField
											name="publisherId"
											label="Publisher"
											clearable											
											searchable
											value={formik.values.publisherId}
											onChange={(selectedValue) => {												
												formik.setFieldValue(
													"publisherId",
													selectedValue as string
												);
											}}
											options={
												publishers?.data?.map(
													(publisher) => ({
														value: publisher.id!,
														label: publisher.name!,
													})
												) || []
											}
										/>
									</Col>
									<Col>
										<SelectField
											name="parentSeries"
											label="Parent Series"
											clearable
											searchable
											value={formik.values.parentSeriesId}
											onChange={(selectedOption) =>
												formik.setFieldValue(
													"parentSeriesId",
													selectedOption as string
												)
											}
											options={
												allSeries
													?.filter(
														(item: Series) =>
															item.id !==
															formik.values.id
													)
													.map((seriesItem) => ({
														value: seriesItem.id!,
														label: seriesItem.name!,
													})) || []
											}
										/>
									</Col>
									<TextAreaField
										name="summary"
										label="Summary"
										rows={3}
										error={
											formik.touched.summary &&
											formik.errors.summary
										}
										value={formik.values.summary}
										onChange={formik.handleChange}
									/>
									<TextAreaField
										name="notes"
										label="Notes"
										rows={3}
										error={
											formik.touched.notes &&
											formik.errors.notes
										}
										value={formik.values.notes}
										onChange={formik.handleChange}
									/>
								</Row>
							</Form>
						</Container>
					</Tab>
					<Tab eventKey="metadata" title="Metadata"></Tab>
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
