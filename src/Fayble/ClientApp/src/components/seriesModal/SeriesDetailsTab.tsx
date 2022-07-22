import { NumberField } from "components/form/numberField";
import { SelectField } from "components/form/selectField";
import { TextField } from "components/form/textField";
import { Series } from "models/api-models";
import { Col, Container, Form, Row } from "react-bootstrap";
import { useFormContext } from "react-hook-form";
import { useParams } from "react-router-dom";
import { useLibrarySeries, usePublishers } from "services";
import { TextAreaField } from "textAreaField";

export const SeriesDetailsTab = () => {
  const { libraryId } = useParams<{ libraryId: string }>();
  const publishers = usePublishers();
  const { data: allSeries } = useLibrarySeries(libraryId!);
  const { getValues } = useFormContext();

  const id = getValues("id");

  return (
    <Container>
      <Form>
        <TextField name="name" label="Name" lockable />
        <Row>
          <Col>
            <NumberField name="year" label="Year" lockable />
          </Col>
          <Col>
            <TextField name="volume" label="Volume" lockable />
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
                  ?.filter((item: Series) => item.id !== id)
                  .map((seriesItem) => ({
                    value: seriesItem.id!,
                    label: seriesItem.name!,
                  })) || []
              }
            />
          </Col>
          <TextAreaField name="summary" label="Summary" rows={3} lockable />
          <TextAreaField name="notes" label="Notes" rows={3} lockable />
        </Row>
      </Form>
    </Container>
  );
};
