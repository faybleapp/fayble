import { faTrashCan } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";
import { Book } from "models/api-models";
import { ImageTypes } from "models/ui-models";
import { BookCoverOverlay } from "pages/library/series/BookCoverOverlay";
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
					className={cn(styles.coverImage, "shadow", {
						[styles.deleted]: book.isDeleted,
						[styles.overlay]: !book.isDeleted,
					})}
					src={`/api/media?id=${book?.id}&mediaRoot=${book?.mediaRoot}&fileName=${ImageTypes.CoverSm}`}
				/>
				<Link
					key={book.id}
					to={`/library/${libraryId}/series/${seriesId}/book/${book.id}`}>
					<BookCoverOverlay
						hideMenu={hideMenu === true}
						edit={() => edit(book.id!)}
						markRead={() => markRead(book.id!)}
						deleted={book.isDeleted}
					/>
				</Link>
				{book.isDeleted ? (
					<div className={styles.deletedIcon}>
						<FontAwesomeIcon icon={faTrashCan} size="lg" />
					</div>
				) : null}
			</div>

			<Figure.Caption className={styles.caption}>
				<Link
					className={styles.link}
					key={book.id}
					to={`/library/${libraryId}/series/${seriesId}/book/${book.id}`}>
					<div className={styles.title}>{title}</div>
					<div className={styles.subtitle}>{subtitle}</div>
				</Link>
			</Figure.Caption>
		</Figure>
	);
};
