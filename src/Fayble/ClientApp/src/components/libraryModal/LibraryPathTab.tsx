import { SanitisePath } from "helpers/pathHelpers";
import React, { useState } from "react";
import { Button, Form, InputGroup } from "react-bootstrap";
import { useFormContext } from "react-hook-form";
import { toast } from "react-toastify";
import { usePathExists } from "services/fileSystem";
import styles from "./LibraryPathsTab.module.scss";

export const LibraryPathTab = () => {
  const [isValidatingPath, setValidatingPath] = useState(false);
  const [newPath, setNewPath] = useState("");
  const { mutate: validatePath } = usePathExists();

  const {
    register,
    setValue,
    watch,
    formState: { errors },
  } = useFormContext();

  const field = register("folderPath");
  const folderPath = watch("folderPath");

  const removePath = () => {
    setValue("folderPath", "", { shouldDirty: true });
  };

  const addPath = async () => {
    setValidatingPath(true);

    const sanitisedPath = SanitisePath(newPath);

    validatePath([null, { path: sanitisedPath }], {
      onSuccess: (exists) => {
        if (!exists) {
          toast.error("Path does not exist or is not accessible");
          setValidatingPath(false);
          return;
        } else {
          setValue("folderPath", newPath, { shouldDirty: true });

          setNewPath("");
          setValidatingPath(false);
        }
      },
      onError: () => {
        toast.error("Path does not exist or is not accessible");
        setValidatingPath(false);
      },
    });
  };

  return (
    <div className={styles.path}>
      {!folderPath ? (
        <>
          <Form.Group className={"mb-3"}>
            <InputGroup>
              <Form.Control
                disabled={isValidatingPath}
                onChange={(e: React.ChangeEvent<HTMLInputElement>): void =>
                  setNewPath(e.target.value)
                }
                value={newPath}
                placeholder="Path"
              />
              <Button
                disabled={isValidatingPath || newPath.trim() === ""}
                onClick={() => addPath()}
                variant="outline-secondary">
                {isValidatingPath ? "Validating..." : "Add Path"}
              </Button>
            </InputGroup>
          </Form.Group>
        </>
      ) : (
        <InputGroup className={styles.pathItem}>
          <Form.Control disabled placeholder="Path" value={folderPath} />
          <Button onClick={() => removePath()} variant="danger">
            X
          </Button>
        </InputGroup>
      )}
    </div>
  );
};
