import { PageContainer } from "components/pageContainer";
import { SanitisePath } from "helpers/pathHelpers";
import { ComicFile } from "models/api-models";
import { useState } from "react";
import { toast } from "react-toastify";
import { usePathExists } from "services/fileSystem";
import { useScanImportFiles } from "services/import";
import { ImportPath } from "./ImportPath";
import { ImportTable } from "./ImportTable";

interface ImportProps {}

export const Import = ({}: ImportProps) => {
  const [isSearching, setIsSearching] = useState(false);
  const [files, setFiles] = useState<ComicFile[]>();
  const { mutate: scanImportFiles } = useScanImportFiles();
  const { mutate: validatePath } = usePathExists();

  const handleSearch = async (path: string) => {
    const sanitisedPath = SanitisePath(path);
    setIsSearching(true);
    setFiles(undefined);
    validatePath([null, { path: sanitisedPath }], {
      onSuccess: (exists) => {
        if (!exists) {
          toast.error("Path does not exist or is not accessible");
          setIsSearching(false);
          return;
        }
        scanImportFiles([null, { path: sanitisedPath }], {
          onError: () => {
            toast.error("Path does not exist or is not accessible");
          },
          onSuccess: (results) => {
            setFiles(results);
          },
          onSettled: () => {
            setIsSearching(false);
          },
        });
      },
      onError: () => {
        toast.error("An error occured while validating path");
        setIsSearching(false);
        return;
      },
    });
  };

  return (
    <PageContainer>
      <ImportPath onSearch={handleSearch} isSearching={isSearching} />
      {files && <ImportTable files={files} />}
    </PageContainer>
  );
};