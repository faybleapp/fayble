import { DatePickerField } from "components/form/datePickerField";
import { MultiSelectField } from "components/form/multiSelectField";
import { SelectField } from "components/form/selectField";
import { TextField } from "components/form/textField";
import { FormikProps } from "formik";
import { languageSelectOptions } from "helpers/languageHelpers";
import { Book, Tag } from "models/api-models";
import React from "react";
import { Col, Container, Form, Row } from "react-bootstrap";
import { useBookTags } from "services/tag";
import { TextAreaField } from "textAreaField";

interface BookDetailsTabProps {
	book: Book;
	formik: FormikProps<Book>;
}

export const BookDetailsTab = ({ formik }: BookDetailsTabProps) => {
	const tags = useBookTags();
	var tagList = Array.from(
		new Set(
			tags?.data
				?.map((tag: Tag) => tag.name)
				.concat(formik?.values?.tags?.map((tag) => tag)) || []
		)
	);

	return (
		<Container>
			<Form>
				<TextField
					name="title"
					label="Title"
					value={formik.values.title}
					onChange={formik.handleChange}
				/>
				<Row>
					<Col>
						<TextField
							name="number"
							label="Number"
							value={formik.values.number}
							onChange={formik.handleChange}
						/>
					</Col>
					<Col>
						<SelectField
							name="language"
							label="Language"
							clearable
							searchable
							value={formik.values.language}
							options={languageSelectOptions}
							onChange={(selectedValue) => {												
								formik.setFieldValue(
									"language",
									selectedValue as string
								);
							}}
						/>
					</Col>
				</Row>
				<Row>
					<Col>
						<DatePickerField
							name="releaseDate"
							label="Release Date"
							onChange={formik.handleChange}
							value={formik.values.releaseDate}
						/>
					</Col>
					<Col>
						<DatePickerField
							name="coverDate"
							label="Cover Date"
							type="month"
							onChange={formik.handleChange}
							value={formik.values.coverDate}
						/>
					</Col>
				</Row>
				<TextAreaField
					name="summary"
					label="Summary"
					value={formik.values.summary}
					onChange={formik.handleChange}
					rows={3}
				/>
				<MultiSelectField
					name="tags"
					creatable
					clearable
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
