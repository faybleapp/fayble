import { yupResolver } from "@hookform/resolvers/yup";
import { Form } from "components/form/Form";
import { NumberField } from "components/form/numberField";
import { TextField } from "components/form/textField/TextField";
import { Series } from "models/api-models";
import { MetadataSearchQuery } from "models/ui-models";
import { Button, Col, Container, Row } from "react-bootstrap";
import { SubmitHandler, useForm } from "react-hook-form";
import { useHttpClient } from "services/httpClient";
import * as yup from "yup";

interface SeriesMetadataTabProps {
	series: Series;
}

export const SeriesMetadataTab = ({ series }: SeriesMetadataTabProps) => {
	const client = useHttpClient();

	const validationSchema = yup
		.object()
		.shape({
			name: yup.string().required("A search query is required"),
			year: yup.number().min(1000).max(9999),
		});

	const methods = useForm<MetadataSearchQuery>({
		resolver: yupResolver(validationSchema),
		defaultValues: { name: series.name, year: series.year },
	});

	const onSubmit: SubmitHandler<MetadataSearchQuery> = async (values, t) => {
		// const results = await client.get<SeriesSearchResult[]>(
		// 	`/metadata/searchseries?name=${values.name}&year=${values.year}`
		// );
		console.log(methods.formState.errors);
		console.log(values);
	};

	return (
		<Container>
			<Form<MetadataSearchQuery> methods={methods} onSubmit={onSubmit}>
				<Row>
					<Col xs={9}>
						<TextField name="name" placeholder="name" />
					</Col>
					<Col xs={3}>
						<NumberField
							onChange={() => {}}
							name="year"
							placeholder="Year"
						/>
					</Col>
				</Row>
				<Button type="submit" size="sm" style={{ width: "100%" }}>
					Search
				</Button>
			</Form>
		</Container>
	);
};
