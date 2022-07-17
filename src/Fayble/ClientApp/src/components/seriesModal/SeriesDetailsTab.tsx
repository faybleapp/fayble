import { NumberField } from "components/form/numberField";
import { SelectField } from "components/form/selectField";
import { OldTextField } from "components/form/textField";
import { FormikProps } from "formik";
import { Series } from "models/api-models";
import { Col, Container, Form, Row } from "react-bootstrap";
import { useParams } from "react-router-dom";
import { useLibrarySeries, usePublishers } from "services";
import { TextAreaField } from "textAreaField";

interface SeriesDetailsTabProps {	
	formik: FormikProps<Series>;
}

export const SeriesDetailsTab = ({
	formik,
}: SeriesDetailsTabProps) => {
	const { libraryId } = useParams<{ libraryId: string }>();
	const publishers = usePublishers();
	const { data: allSeries } = useLibrarySeries(libraryId!);

	return (
		<Container>
			<Form>
				<OldTextField
					name="name"
					label="Name"
					locked={formik.values.fieldLocks.name}
					onLock={(lock: boolean) =>
						formik.setFieldValue("fieldLocks.name", lock)
					}
					error={formik.touched?.name && formik.errors?.name}
					value={formik.values.name}
					onChange={formik.handleChange}
				/>
				<Row>
					<Col>
						<NumberField
							name="year"
							label="Year"
							locked={formik.values.fieldLocks.year}
							onLock={(lock: boolean) =>
								formik.setFieldValue("fieldLocks.year", lock)
							}
							error={formik.touched.year && formik.errors.year}
							value={formik.values.year}
							onChange={formik.handleChange}
						/>
					</Col>
					<Col>
						<OldTextField
							name="volume"
							label="Volume"
							locked={formik.values.fieldLocks.volume}
							onLock={(lock: boolean) =>
								formik.setFieldValue("fieldLocks.volume", lock)
							}
							error={
								formik.touched.volume && formik.errors.volume
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
							options={
								publishers?.data?.map((publisher) => ({
									value: publisher.id!,
									label: publisher.name!,
								})) || []
							}
						/>
					</Col>
					<Col>
						<SelectField
							name="parentSeriesId"
							label="Parent Series"
							clearable
							searchable							
							
							options={
								allSeries
									?.filter(
										(item: Series) =>
											item.id !== formik.values.id
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
						locked={formik.values.fieldLocks.summary}
						onLock={(lock: boolean) =>
							formik.setFieldValue("fieldLocks.summary", lock)
						}
						error={formik.touched.summary && formik.errors.summary}
						value={formik.values.summary}
						onChange={formik.handleChange}
					/>
					<TextAreaField
						name="notes"
						label="Notes"
						rows={3}
						locked={formik.values.fieldLocks.notes}
						onLock={(lock: boolean) =>
							formik.setFieldValue("fieldLocks.notes", lock)
						}
						error={formik.touched.notes && formik.errors.notes}
						value={formik.values.notes}
						onChange={formik.handleChange}
					/>
				</Row>
			</Form>
		</Container>
	);
};
