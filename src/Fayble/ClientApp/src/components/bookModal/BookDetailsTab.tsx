import { DatePickerField } from "components/form/datePickerField";
import { TextField } from "components/form/textField";
import { FormikProps } from "formik";
import { Book } from "models/api-models";
import React from "react";
import { Col, Container, Form, Row } from "react-bootstrap";
import { TextAreaField } from "textAreaField";

interface BookDetailsTabProps {
	book: Book;
	formik: FormikProps<Book>;
}

export const BookDetailsTab = ({ formik }: BookDetailsTabProps) => {
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
						<DatePickerField
							name="coverDate"
							label="Cover Date"
							onChange={formik.handleChange}
							value={formik.values.coverDate}
						/>
					</Col>
					<Col>
						<DatePickerField
							name="storeDate"
							label="Store Date"
							error={
								formik.touched.storeDate &&
								formik.errors.storeDate
							}
							onChange={formik.handleChange}
							value={formik.values.storeDate}
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

				<TextAreaField
					name="review"
					label="Review"
					value={formik.values.review}
					onChange={formik.handleChange}
					rows={3}
				/>
				<TextAreaField
					name="notes"
					label="Notes"
					value={formik.values.notes}
					onChange={formik.handleChange}
					rows={3}
				/>
			</Form>
		</Container>
	);
};
