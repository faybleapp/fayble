import { Book } from "models/api-models";
import React from "react";
import { Modal } from "react-bootstrap";

interface BookModalProps {
	book: Book;
	show: boolean;
	close: () => void;
}

export const BookModal = ({ close, show, book }: BookModalProps) => {
	return (
		<Modal size="lg" show={show} onHide={close}>
			<Modal.Header closeButton>
				<Modal.Title>
					Edit {book.mediaType.replace(/([A-Z])/g, " $1").trim()}
				</Modal.Title>
			</Modal.Header>
			<Modal.Body></Modal.Body>
		</Modal>
	);
};
