import { DatePickerField } from "components/form/datePickerField";
import { MultiSelectField } from "components/form/multiSelectField";
import { SelectField } from "components/form/selectField";
import { TextField } from "components/form/textField";
import { FormikProps } from "formik";
import { languageSelectOptions } from "helpers/languageHelpers";
import { Book, Tag } from "models/api-models";
import { Col, Container, Form, Row } from "react-bootstrap";
import { useBookTags } from "services/tag";
import { TextAreaField } from "textAreaField";

interface BookDetailsTabProps {	
	formik: FormikProps<Book>;
}

export const BookDetailsTab = ({ formik }: BookDetailsTabProps) => {
	const tags = useBookTags();
	var tagList = Array.from(
		new Set(
			tags?.data
				?.map((tag: Tag) => tag.name)
				.concat(formik?.values?.tags?.map((tag) => tag) || []) || []
		)
	);

	return (
		<Container>
			<Form>
				<TextField
					name="title"
					label="Title"
					lockable
				/>
				<Row>
					<Col>
						<TextField
							name="number"
							label="Number"
							lockable
						/>
					</Col>
					<Col>
						<SelectField
							name="language"
							label="Language"
							clearable							
							lockable
							searchable						
							options={languageSelectOptions}
							
						/>
					</Col>
				</Row>
				<Row>
					<Col>
						<DatePickerField
							name="releaseDate"
							label="Release Date"
							locked={formik.values.fieldLocks.releaseDate}
							onLock={(lock: boolean) =>
								formik.setFieldValue(
									"fieldLocks.releaseDate",
									lock
								)
							}
							onChange={formik.handleChange}
							value={formik.values.releaseDate}
						/>
					</Col>
					<Col>
						<DatePickerField
							name="coverDate"
							label="Cover Date"
							type="month"
							locked={formik.values.fieldLocks.coverDate}
							onLock={(lock: boolean) =>
								formik.setFieldValue(
									"fieldLocks.coverDate",
									lock
								)
							}
							onChange={formik.handleChange}
							value={formik.values.coverDate}
						/>
					</Col>
				</Row>
				<TextAreaField
					name="summary"
					label="Summary"
					locked={formik.values.fieldLocks.summary}
					onLock={(lock: boolean) =>
						formik.setFieldValue("fieldLocks.summary", lock)
					}
					value={formik.values.summary}
					onChange={formik.handleChange}
					rows={3}
				/>
				<MultiSelectField
					name="tags"
					creatable
					clearable
					locked={formik.values.fieldLocks.tags}
					onLock={(lock: boolean) =>
						formik.setFieldValue("fieldLocks.tags", lock)
					}
					value={formik.values.tags || []}
					label="Tags"					
					onChange={(selectedValues) => {
						formik.setFieldValue(
							"tags",
							selectedValues?.map((option) => option.value)
						);
					}}
					options={tagList.map((tag) => ({ value: tag, label: tag }))}
				/>
			</Form>
		</Container>
	);
};
