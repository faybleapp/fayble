import cn from "classnames";
import { Book } from "models/api-models";
import { MediaType } from "models/ui-models";
import React from "react";
import { Figure } from "react-bootstrap";
import { Link, useParams } from "react-router-dom";
import styles from "./BookCoverGrid.module.scss";
import { BookCoverOverlay } from "./BookCoverOverlay";

interface BookCoverGridProps {
	books: Book[];
	title?: string
}

export const BookCoverGrid = ({ books, title }: BookCoverGridProps) => {
	const { libraryId, seriesId } = useParams<{
		libraryId: string;
		seriesId: string;
	}>();

	const markRead = (id: string) => {};

	const edit = (id: string) => {};

	return (
		<>
			<div className={styles.gridTitle}>
				<h6>{title}</h6>
			</div>
			{books &&
				books.map((book) => {
					return (
						<Figure key={book.id} className={styles.series}>
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
									<div className={styles.title}>
										{book.mediaType ===
										MediaType[MediaType.Book]
											? book.title
											: `Issue #${book.number.padStart(
													3,
													"0"
											  )}`}
									</div>
									<div className={styles.subtitle}>
										{book.pageCount} pages
									</div>
								</Link>
							</Figure.Caption>
						</Figure>
					);
				})}
		</>
	);
};
