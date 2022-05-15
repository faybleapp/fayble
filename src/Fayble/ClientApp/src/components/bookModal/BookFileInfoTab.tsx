import { Book } from "models/api-models";
import { Container } from "react-bootstrap";
import styles from "./BookFileInfoTab.module.scss";

interface BookFileInfoTabProps {
	book: Book;
}

export const BookFileInfoTab = ({ book }: BookFileInfoTabProps) => {
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
