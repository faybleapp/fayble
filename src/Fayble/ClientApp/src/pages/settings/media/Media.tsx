import { Form } from "components/form";
import { TextField } from "components/form/textField";
import { PageContainer } from "components/pageContainer";
import { MediaSettings } from "models/api-models";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { useMediaSettings, useUpdateMediaSettings } from "services/setting";

interface MediaProps {}

export const Media = ({}: MediaProps) => {
  const { data: mediaSettings, isLoading } = useMediaSettings();
  const updateMediaSettings = useUpdateMediaSettings();

  const form = useForm<MediaSettings>({    
    
  });

  useEffect(() => {
    mediaSettings && form.reset(mediaSettings);
  }, [form, mediaSettings]);

  return (
    <PageContainer>
      <div>
        <h4>Book Naming</h4>
        <hr />
        <Form<MediaSettings> form={form} onSubmit={() => {}} >
          <TextField
            name="comicBookStandardNamingConvention"
            label="Comic Book Standard format"
          />
          <TextField
            name="comicBookOneShotNamingConvention"
            label="Comic Book One-Shot format"
          />
        </Form>
      </div>
    </PageContainer>
  );
};
