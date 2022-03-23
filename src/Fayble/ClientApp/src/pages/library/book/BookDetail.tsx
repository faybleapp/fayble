import { faStar as faStarO } from "@fortawesome/free-regular-svg-icons";
import { faStar } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Book } from "models/api-models";
import React, { useEffect, useState } from "react";
import { Image } from "react-bootstrap";
import Rating from "react-rating";
import ShowMoreText from "react-show-more-text";
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
				src={
					book &&
					`/api/media/${encodeURIComponent(book.media?.coverSm!)}`
				}
			/>
			<div className={styles.detailsPanel}>
				<div className={styles.detailsTitle}>
					<h4>
						{!!book.title
							? book.title
							: `Issue #${book?.number.padStart(3, "0")}`}
					</h4>

					<div className={styles.rating}>
						<Rating
							className={styles.rating}
							fractions={2}
							onChange={ratingChanged}
							initialRating={book?.rating}
							emptySymbol={
								<FontAwesomeIcon
									icon={faStarO}
									color={"#fafafa"}
								/>
							}
							fullSymbol={
								<FontAwesomeIcon
									icon={faStar}
									color={"#fafafa"}
								/>
							}
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

					{book?.storeDate && (
						<div className={styles.detailProperty}>
							<div className={styles.detailsHeading}>
								Store Date
							</div>
							<div>{book?.number}</div>
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
							<div>{book?.publisher}</div>
						</div>
					)}
				</div>
				<div className={styles.summary}>
					<ShowMoreText
						lines={3}
						more="show more"
						less="show less"
						anchorClass=""
						width={750}>
						{book?.summary}
					</ShowMoreText>
				</div>
			</div>
		</div>
	);
};
