import { Button, Spinner } from "react-bootstrap";
import { DeleteConfirmationPopover } from "../deleteConfirmationPopover";
import styles from "./LibraryModalFooter.module.scss";

interface LibraryModalFooterProps {
  isNew: boolean;
  deleteLibrary: () => void;
  createLibrary: () => void;
  updateLibrary: () => void;
  isDeleting: boolean;
  isCreating: boolean;
  isUpdating: boolean;
  continueDisabled: boolean;
  activeTabKey: string;
  setActiveTabKey: (key: string) => void;
  close: () => void;
}

export const LibraryModalFooter = ({
  isNew,
  deleteLibrary,
  createLibrary,
  updateLibrary,
  isDeleting,
  isUpdating,
  isCreating,
  continueDisabled,
  activeTabKey,
  setActiveTabKey,
  close,
}: LibraryModalFooterProps) => {
  return (
    <>
      {!isNew ? (
        <DeleteConfirmationPopover
          placement="right"
          onConfirmation={deleteLibrary}
          title="Delete library">
          <Button
            className={styles.deleteButton}
            variant="danger"
            disabled={isDeleting}>
            {isDeleting ? (
              <>
                <Spinner
                  as="span"
                  animation="border"
                  size="sm"
                  role="status"
                  aria-hidden="true"
                />
                {"  Deleting..."}
              </>
            ) : (
              "Delete"
            )}
          </Button>
        </DeleteConfirmationPopover>
      ) : null}
      <Button variant="secondary" onClick={close}>
        {isNew ? "Cancel" : "Close"}
      </Button>
      {activeTabKey === "1" || !isNew ? null : (
        <Button
          variant="primary"
          onClick={() =>
            setActiveTabKey(
              activeTabKey && (parseInt(activeTabKey) - 1).toString()
            )
          }>
          Back
        </Button>
      )}
      {isNew ? (
        activeTabKey === "3" ? (
          <Button variant="primary" onClick={createLibrary}>
            Create
          </Button>
        ) : (
          <Button
            variant="primary"
            disabled={continueDisabled}
            onClick={() =>
              setActiveTabKey(
                activeTabKey && (parseInt(activeTabKey) + 1).toString()
              )
            }>
            Next
          </Button>
        )
      ) : (
        <Button
          variant="primary"
          disabled={isUpdating || continueDisabled}
          onClick={updateLibrary}>
          {isUpdating ? (
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
            "Save Changes"
          )}
        </Button>
      )}
    </>
  );
};
