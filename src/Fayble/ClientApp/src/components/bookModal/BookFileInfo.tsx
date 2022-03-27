import { ReadOnlyField } from "components/form/readOnlyField";
import { Book } from "models/api-models";
import React from "react";
import { Container } from "react-bootstrap";

interface BookFileInfoProps {
	book: Book;
}

export const BookFileInfo = ({ book }: BookFileInfoProps) => {
	return (
		<Container>
			<ReadOnlyField label="Filename" value={book.filename} />
		</Container>
	);
};
