import cn from "classnames";
import { Book } from "models/api-models";
import { BookCoverOverlay } from "pages/library/series/BookCoverOverlay";
import React from "react";
import { Figure } from "react-bootstrap";
import { Link, useParams } from "react-router-dom";
import styles from "./BookItem.module.scss";

interface BookItemProps {
	book: Book;
	title: string;
	subtitle?: string;
	hideMenu?: boolean;
}

export const BookItem = ({
	book,
	hideMenu,
	title,
	subtitle,
}: BookItemProps) => {
	const { libraryId, seriesId } = useParams<{
		libraryId: string;
		seriesId: string;
	}>();

	const markRead = (id: string) => {};

	const edit = (id: string) => {};

	return (
		<Figure key={book.id} className={styles.book}>
			<div className={styles.coverContainer}>
				<Figure.Image
					className={cn(styles.coverImage, "shadow")}
					src={`/api/media/${encodeURIComponent(
						book?.media?.coverSm || ""
					)}`}
				/>

				<div className={styles.overlay}>
					<Link
						key={book.id}
						to={`/library/${libraryId}/series/${seriesId}/book/${book.id}`}>
						<BookCoverOverlay
							hideMenu={hideMenu === true}
							edit={() => edit(book.id!)}
							markRead={() => markRead(book.id!)}
						/>
					</Link>
				</div>
			</div>

			<Figure.Caption className={styles.caption}>
				<Link
					className={styles.link}
					key={book.id}
					to={`/library/${libraryId}/series/${seriesId}/book/${book.id}`}>
					<div className={styles.title}>{title}</div>
					<div className={styles.subtitle}>{subtitle }</div>
				</Link>
			</Figure.Caption>
		</Figure>
	);
};
