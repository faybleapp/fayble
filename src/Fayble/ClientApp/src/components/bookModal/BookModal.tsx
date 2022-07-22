import { Form } from "components/form";
import { ModalTabs } from "components/modalTabs";
import { Book } from "models/api-models";
import { Button, Modal, Spinner, Tab } from "react-bootstrap";
import { SubmitHandler, useForm } from "react-hook-form";
import { useUpdateBook } from "services/book";
import { BookDetailsTab } from "./BookDetailsTab";
import { BookFileInfoTab } from "./BookFileInfoTab";
import styles from "./BookModal.module.scss";
import { BookPeopleTab } from "./BookPeopleTab";

interface BookModalProps {
  book: Book;
  show: boolean;
  close: () => void;
}

export const BookModal = ({ book, show, close }: BookModalProps) => {
  const updateBook = useUpdateBook();

  const form = useForm<Book>({
    defaultValues: book,
  });

  const handleSubmit: SubmitHandler<Book> = (values) => {
    updateBook.mutate([values.id, values], {
      onSuccess: () => {
        close();
      },
    });
  };

  const onExited = () => form.reset(book);

  return (
    <Modal size="lg" show={show} onHide={close} onExited={onExited}>
      <Modal.Header closeButton>
        <Modal.Title>
          Edit {book.mediaType.replace(/([A-Z])/g, " $1").trim()}
        </Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form<Book> form={form}>
          <ModalTabs defaultActiveKey="details">
            <Tab eventKey="details" title="Details">
              <BookDetailsTab />
            </Tab>
            <Tab eventKey="people" title="People">
              <BookPeopleTab />
            </Tab>
            <Tab eventKey="metadata" title="Metadata"></Tab>
            <Tab eventKey="fileInfo" title="File Info">
              <BookFileInfoTab book={book} />
            </Tab>
          </ModalTabs>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={close}>
          Close
        </Button>
        <Button
          variant="primary"
          onClick={form.handleSubmit(handleSubmit)}
          disabled={updateBook.isLoading || !form.formState.isDirty}>
          {updateBook.isLoading ? (
            <>
              <Spinner
                className={styles.spinner}
                as="span"
                animation="border"
                size="sm"
                role="status"
                aria-hidden="true"
              />
              Saving...
            </>
          ) : (
            "Save Changes"
          )}
        </Button>
      </Modal.Footer>
    </Modal>
  );
};
