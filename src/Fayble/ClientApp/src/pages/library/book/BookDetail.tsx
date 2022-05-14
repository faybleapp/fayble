import { Image } from "components/image";
import { StarRating } from "components/starRating";
import { Book } from "models/api-models";
import { ImageTypes } from "models/ui-models";
import { useEffect, useState } from "react";
import { useUpdateBook } from "services/book";
import styles from "./BookDetail.module.scss";

interface BookDetailProps {
	book: Book;
}

export const BookDetail = (props: BookDetailProps) => {
	const [book, setBook] = useState<Book>(props.book);

	useEffect(() => {
		setBook(props.book);
	}, [props.book]);

	const updateBook = useUpdateBook();

	const ratingChanged = (rating: number) => {
		const updatedBook = { ...book, rating: rating };
		updateBook.mutate([book.id, updatedBook]);
		setBook(updatedBook);
	};

	return (
		<div className={styles.container}>
			<Image
				className={styles.cover}
				src={`/api/media?id=${book?.id}&mediaRoot=${book?.mediaRoot}&filename=${ImageTypes.CoverSm}`}
			/>
			<div className={styles.detailsPanel}>
				<div className={styles.detailsTitle}>
					<h4>
						{!!book.title
							? book.title
							: `Issue #${book?.number.padStart(3, "0")}`}
					</h4>

					<div className={styles.rating}>
						<StarRating
							rating={book.rating}
							onChange={ratingChanged}
						/>
					</div>
				</div>

				<hr />
				<div className={styles.detailSection}>
					{book?.number && (
						<div className={styles.detailProperty}>
							<div className={styles.detailsHeading}>Number</div>
							<div>{book?.number}</div>
						</div>
					)}

					{book?.releaseDate && (
						<div className={styles.detailProperty}>
							<div className={styles.detailsHeading}>
								Store Date
							</div>
							<div>{book?.releaseDate}</div>
						</div>
					)}

					{book?.coverDate && (
						<div className={styles.detailProperty}>
							<div className={styles.detailsHeading}>
								Cover Date
							</div>
							<div>{book?.coverDate}</div>
						</div>
					)}

					{book?.publisher && (
						<div className={styles.detailProperty}>
							<div className={styles.detailsHeading}>
								Publisher
							</div>
							<div>{book?.publisher?.name}</div>
						</div>
					)}
				</div>
				<div className={styles.summary}>{book?.summary}</div>
			</div>
		</div>
	);
};
