import { faTrashCan } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";
import { Book, Series } from "models/api-models";
import { ImageTypes } from "models/ui-models";
import { SeriesCoverOverlay } from "pages/library/SeriesCoverOverlay";
import { Figure } from "react-bootstrap";
import { Link } from "react-router-dom";
import styles from "./CoverItem.module.scss";
interface CoverItemProps {
	item: Book | Series;
	title: string;
	firstSubtitle?: string;
	secondSubtitle?: string;
	link: string;
	isDeleted?: boolean;
	menu?: React.ReactNode;
}

const edit = (id: string) => {
	//setSelectedSeriesId(id);
	//setShowEditSeriesModal(true);
};

const markReadClick = (id: string) => {
	//setSelectedSeriesId(id);
	//setShowMarkReadModal(true);
};

export const CoverItem = ({
	item,
	title,
	firstSubtitle,
	secondSubtitle,
	link,
	isDeleted,
	menu,
}: CoverItemProps) => {
	return (
		<Figure key={item.id} className={styles.item}>
			<div className={styles.coverContainer}>
				<Figure.Image
					className={cn(styles.coverImage, "shadow", {
						[styles.deleted]: isDeleted,
					})}
					src={`/api/media?id=${item?.id}&mediaRoot=${item?.mediaRoot}&fileName=${ImageTypes.CoverSm}`}
				/>
				{isDeleted ? (
					<div className={styles.deletedIcon}>
						<FontAwesomeIcon icon={faTrashCan} size="lg" />
					</div>
				) : null}
				<div className={styles.overlay}>
					<Link key={item.id} to={link}>
						<SeriesCoverOverlay
							edit={() => edit(item.id)}
							markRead={() => markReadClick(item.id)}
						/>
					</Link>
				</div>
			</div>
			<Figure.Caption className={styles.caption}>
				<Link className={styles.link} key={item.id} to={link}>
					<div className={styles.title}>{title}</div>
				</Link>
				{firstSubtitle && (
					<div className={styles.subtitle}>{firstSubtitle}</div>
				)}
				{secondSubtitle && (
					<div className={styles.subtitle}>{secondSubtitle}</div>
				)}
			</Figure.Caption>
		</Figure>
	);
};
