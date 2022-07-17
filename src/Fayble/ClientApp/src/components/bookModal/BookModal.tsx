import { Form } from "components/form";
import { ModalTabs } from "components/modalTabs";
import { useFormik } from "formik";
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

	const methods = useForm<Book>({
		defaultValues: book,
	});

	const formik = useFormik<Book>({
		initialValues: book,
		enableReinitialize: true,
		onSubmit: (values: Book) => {
			updateBook.mutate([values.id, values], {
				onSuccess: () => {
					close();
				},
			});
		},
		validateOnMount: true,
	});

	const onSubmit: SubmitHandler<Book> = (values) =>
		updateBook.mutate([values.id, values], {
			onSuccess: () => {
				close();
			},
		});

	const onExited = () => formik.resetForm();

	return (
		<Modal size="lg" show={show} onHide={close} onExited={onExited}>
			<Modal.Header closeButton>
				<Modal.Title>
					Edit {book.mediaType.replace(/([A-Z])/g, " $1").trim()}
				</Modal.Title>
			</Modal.Header>
			<Modal.Body>
				<Form<Book> methods={methods} onSubmit={onSubmit}>
					<ModalTabs defaultActiveKey="details">
						<Tab eventKey="details" title="Details">
							<BookDetailsTab formik={formik} />
						</Tab>
						<Tab eventKey="people" title="People">
							<BookPeopleTab formik={formik} />
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
					onClick={formik.submitForm}
					disabled={updateBook.isLoading || !formik.dirty}>
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
