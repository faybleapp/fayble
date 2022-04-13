import { Book } from "models/api-models";
import React from "react";
import { Container } from "react-bootstrap";
import styles from "./BookFileInfo.module.scss";

interface BookFileInfoProps {
	book: Book;
}

export const BookFileInfo = ({ book }: BookFileInfoProps) => {
	return (
		<Container>
			<div className={styles.label}>File Name</div>
			<div className={styles.value}>{book.fileName}</div>
			<br />
			<div className={styles.label}>File Path</div>
			<div className={styles.value}>{book.filePath}</div>
			<br />
			<div className={styles.label}>File Size</div>
			<div className={styles.value}>{`${book.fileSize}mb`}</div>
			<br />
		</Container>
	);
};
