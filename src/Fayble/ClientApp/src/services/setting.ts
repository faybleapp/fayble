import { MediaSettings } from "models/api-models";
import { useQueryClient } from "react-query";
import { toast } from "react-toastify";
import { useApiMutation } from "./useApiMutation";
import { useApiQuery } from "./useApiQuery";

export const useMediaSettings = () =>
  useApiQuery<MediaSettings>(["settings", "media"], `/settings/media`);

  export const useUpdateMediaSettings = () => {
    const queryClient = useQueryClient();
    return useApiMutation<MediaSettings, MediaSettings>(
      "POST",
      () => `/settings/media`,
      {
        onSuccess: () => {
          queryClient.invalidateQueries(["settings", "media"]);          
        },
        onError: () => {
          toast.error("An error occurred while updating settings");
        },
      }
    );
  };
  