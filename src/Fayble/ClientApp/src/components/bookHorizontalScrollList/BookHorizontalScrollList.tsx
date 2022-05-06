import {
	faChevronLeft,
	faChevronRight
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { BookItem } from "components/bookItem";
import { BookModal } from "components/bookModal";
import { Book } from "models/api-models";
import { MediaType } from "models/ui-models";
import React, { useEffect, useRef, useState } from "react";
import { Button } from "react-bootstrap";
import styles from "./BookHorizontalScrollList.module.scss";

interface BookHorizontalScrollListProps {
	title: string;
	books: Book[];
}

export const BookHorizontalScrollList = ({
	title,
	books,
}: BookHorizontalScrollListProps) => {
	const scrollListRef = useRef<any>(null);

	const [selectedBook] = useState<Book>();
	const [showBookModal, setShowBookModal] = useState<boolean>(false);
	const [showControls, setShowControls] = useState<boolean>(false);

	const handleScroll = (direction: "right" | "left") => {
		const contentWidth =
			document.getElementById("main-page-content")!.offsetWidth - 200;
		const element: any = scrollListRef.current;
		element?.scrollBy({
			top: 0,
			left: direction === "right" ? contentWidth : contentWidth * -1,
			behavior: "smooth",
		});
	};

	useEffect(() => {
		setShowControls(
			scrollListRef.current?.scrollWidth >
				scrollListRef.current?.clientWidth
		);
	}, []);

	return (
		<>
			<div className={styles.listHeader}>
				<div className={styles.listTitle}>
					<h6>{title}</h6>
				</div>
				{showControls && (
					<div className={styles.scrollButtons}>
						<Button
							className={styles.icon}
							onClick={() => handleScroll("left")}>
							<FontAwesomeIcon
								className={styles.icon}
								icon={faChevronLeft}
							/>
						</Button>
						<Button
							className={styles.icon}
							onClick={() => handleScroll("right")}>
							<FontAwesomeIcon icon={faChevronRight} />
						</Button>
					</div>
				)}
			</div>
			<div className={styles.list} ref={scrollListRef} id="list">
				{books.map((book) => (
					<BookItem
						book={book}
						title={
							book.mediaType === MediaType.Book
								? book.title
								: `${book.series?.name} (${
										book.series?.year || book.series?.volume
								  })`
						}
						subtitle={
							book.mediaType === MediaType.Book
								? undefined
								: `Issue #${book.number.padStart(3, "0")}`
						}
						hideMenu
					/>
				))}
			</div>
			{selectedBook && (
				<BookModal
					show={showBookModal}
					book={selectedBook}
					close={() => setShowBookModal(false)}
				/>
			)}
		</>
	);
};
