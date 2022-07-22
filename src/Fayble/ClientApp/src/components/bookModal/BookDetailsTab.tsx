import { DatePickerField } from "components/form/datePickerField";
import { SelectField } from "components/form/selectField";
import { TextField } from "components/form/textField";
import { MultiSelectField } from "components/multiSelectField";
import { languageSelectOptions } from "helpers/languageHelpers";
import { Tag } from "models/api-models";
import { Col, Container, Row } from "react-bootstrap";
import { useFormContext, useWatch } from "react-hook-form";
import { useBookTags } from "services/tag";
import { TextAreaField } from "textAreaField";

export const BookDetailsTab = () => {
  const allTags = useBookTags();

  const { control, setValue } = useFormContext();
  const tags: string[] = useWatch({ control, name: "tags" });
  const tagsLocked: boolean = useWatch({ control, name: "fieldLocks.tags" });

  var tagList = Array.from(
    new Set(
      allTags?.data
        ?.map((tag: Tag) => tag.name)
        .concat(tags?.map((tag) => tag) || []) || []
    )
  );

  return (
    <Container>
      <TextField name="title" label="Title" lockable />
      <Row>
        <Col>
          <TextField name="number" label="Number" lockable />
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
          <DatePickerField name="releaseDate" label="Release Date" lockable />
        </Col>
        <Col>
          <DatePickerField
            name="coverDate"
            label="Cover Date"
            type="month"
            lockable
          />
        </Col>
      </Row>
      <TextAreaField name="summary" label="Summary" lockable rows={3} />
      <MultiSelectField
        name="tags"
        creatable
        clearable
        lockable
        locked={tagsLocked}
        onLock={(lock: boolean) =>
          setValue("fieldLocks.tags", lock, { shouldDirty: true })
        }
        value={tags}
        onChange={(tags) => setValue("tags", tags, { shouldDirty: true })}
        options={tagList.map((tag) => ({ value: tag, label: tag }))}
        label="Tags"
      />
    </Container>
  );
};
