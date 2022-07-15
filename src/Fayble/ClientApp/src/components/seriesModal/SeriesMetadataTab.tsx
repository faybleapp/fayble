import { NumberField } from "components/form/numberField";
import { TextField } from "components/form/textField";
import { useFormik } from "formik";
import { Series, SeriesSearchResult } from "models/api-models";
import { MetadataSearchQuery } from "models/ui-models";
import { Button, Col, Container, Row } from "react-bootstrap";
import { useHttpClient } from "services/httpClient";

interface SeriesMetadataTabProps {
	series: Series;
}

export const SeriesMetadataTab = ({ series }: SeriesMetadataTabProps) => {

	const client = useHttpClient();
	
	const formik = useFormik<MetadataSearchQuery>({
		initialValues: { name: series.name || "", year: series.year },
		enableReinitialize: true,
		onSubmit: async (values: MetadataSearchQuery) => {
			const results = await client.get<SeriesSearchResult[]>(`/metadata/searchseries?name=${values.name}&year=${values.year}`)
			console.log(results.data);
		},
	});

	return (
		<Container>
			<Row>
				<Col xs={9}>
					<TextField
						name="name"
						placeholder="Name"
						value={formik.values.name}
						onChange={formik.handleChange}
					/>
				</Col>
				<Col xs={3}>
					<NumberField
						name="year"
						placeholder="Year"
						value={formik.values.year}
						onChange={formik.handleChange}
					/>
				</Col>
			</Row>
			<Button
				onClick={formik.submitForm}
				size="sm"
				style={{ width: "100%" }}>
				Search
			</Button>
		</Container>
	);
};
