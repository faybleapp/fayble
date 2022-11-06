import { yupResolver } from "@hookform/resolvers/yup";
import { Form } from "components/form";
import { SelectField } from "components/form/selectField";
import { SwitchField } from "components/form/switchField";
import { TextField } from "components/form/textField";
import { PageContainer } from "components/pageContainer";
import { MediaSettings } from "models/api-models";
import { ColonReplacement } from "models/ColonReplacement";
import { MissingTokenReplacement } from "models/MissingTokenReplacement";
import { useEffect, useState } from "react";
import { Button, Container, Spinner } from "react-bootstrap";
import { SubmitHandler, useForm } from "react-hook-form";
import { useMediaSettings, useUpdateMediaSettings } from "services/setting";
import * as yup from "yup";
import styles from "./Media.module.scss";

interface MediaProps {}

export const Media = ({}: MediaProps) => {
  const [comicBookStandardFormatExample, setComicBookStandardFormatExample] =
    useState<string>("");
  const [comicBookOneShotFormatExample, setComicBookOneShotFormatExample] =
    useState<string>("");
  const { data: mediaSettings, isLoading } = useMediaSettings();
  const updateMediaSettings = useUpdateMediaSettings();

  const validationSchema = yup.object().shape({
    comicBookStandardNamingFormat: yup.string().trim().required("Required"),
  });

  const form = useForm<MediaSettings>({
    resolver: yupResolver(validationSchema),
  });

  const comicBookStandardFormat = form.watch(
    "comicBookStandardNamingFormat"
  );

  const comicBookOneShotFormat = form.watch("comicBookOneShotNamingFormat");

  useEffect(() => {
    mediaSettings && form.reset(mediaSettings);
  }, [form, mediaSettings]);

  useEffect(() => {
    setComicBookStandardFormatExample(generateExample(comicBookStandardFormat));
    setComicBookOneShotFormatExample(generateExample(comicBookOneShotFormat));
  }, [comicBookStandardFormat, comicBookOneShotFormat]);

  const handleSubmit: SubmitHandler<MediaSettings> = (values) =>
    updateMediaSettings.mutate(values);

  const generateExample = (format: string) => {
    let example = format;

    example = example?.replace("{Series Name}", "Star Wars");
    example = example?.replace("{Series Volume}", "2015");

    const bookNumberMatch = example?.match("{Book Number(.*?)}");

    if (bookNumberMatch != null) {
      example = example?.replace(
        bookNumberMatch[0],
        "1".padStart(bookNumberMatch[1]?.replace(":", "").length, "0")
      );
    }

    example = example?.replace("{Book CoverDate:MMM YYYY}", "Mar 2015");
    example = example?.replace("{Book CoverDate:MMM, YYYY}", "Mar, 2015");
    example = example?.replace("{Book CoverDate:MMMM YYYY}", "March 2015");
    example = example?.replace("{Book CoverDate:MMMM, YYYY}", "March, 2015");
    example = example?.replace("{Book CoverDate:MM-YYYY}", "03-2015");
    example = example?.replace("{Book CoverDate:YYYY-MM}", "2015-03");

    return example;
  };

  return (
    <PageContainer loading={isLoading}>
      <div>
        <h4>Book Naming</h4>
        <hr />
        <Container>
          <Form<MediaSettings> form={form} onSubmit={() => {}}>
            <SwitchField name="renameFiles" label="Rename Files" />
            <br />

            {form.watch("renameFiles") && (
              <>
                <TextField
                  name="comicBookStandardNamingFormat"
                  label="Comic Book Standard Format"
                />
                <div className={styles.formatExample}>
                  {comicBookStandardFormatExample}
                </div>
                <TextField
                  name="comicBookOneShotNamingFormat"
                  label="Comic Book One-Shot Format"
                />
                <div className={styles.formatExample}>
                  {comicBookOneShotFormatExample}
                </div>
                <SelectField
                  name="colonReplacement"
                  label="Colon Replacement"
                  options={[
                    {
                      label: "Delete",
                      value: ColonReplacement.Delete,
                    },
                    {
                      label: "Space",
                      value: ColonReplacement.Space,
                    },
                    {
                      label: "Space Dash",
                      value: ColonReplacement.SpaceDash,
                    },
                    {
                      label: "Space Dash Space",
                      value: ColonReplacement.SpaceDashSpace,
                    },
                  ]}
                />
                 <SelectField
                  name="missingTokenReplacement"
                  label="Missing Token Replacement"
                  tooltip="When a token in the naming format is unavailble, this value will be used in its place"
                  options={[
                    {
                      label: "Empty",
                      value: MissingTokenReplacement.Empty,
                    },
                    {
                      label: "Unknown",
                      value: MissingTokenReplacement.Unknown,
                    },
                    {
                      label: "TBA",
                      value: MissingTokenReplacement.TBA,
                    },
                    {
                      label: "TBD",
                      value: MissingTokenReplacement.TBD,
                    },
                  ]}
                />
              </>
            )}
            <Button
              type="submit"
              disabled={
                updateMediaSettings.isLoading || !form.formState.isDirty
              }
              className={styles.saveButton}
              onClick={form.handleSubmit(handleSubmit)}>
              {updateMediaSettings.isLoading ? (
                <>
                  <Spinner
                    as="span"
                    animation="border"
                    size="sm"
                    role="status"
                    aria-hidden="true"
                  />
                  {"  Saving..."}
                </>
              ) : (
                "Save Settings"
              )}
            </Button>
          </Form>
        </Container>
      </div>
    </PageContainer>
  );
};
