import cn from "classnames";
import { Series } from "models/api-models";
import React, { useState } from "react";
import { Figure } from "react-bootstrap";
import { Link, useParams } from "react-router-dom";
import styles from "./SeriesCoverGrid.module.scss";
import { SeriesCoverOverlay } from "./SeriesCoverOverlay";

interface SeriesCoverGridProps {
	items: Series[];
}

export const SeriesCoverGrid = ({ items }: SeriesCoverGridProps) => {
	const { libraryId } = useParams<{ libraryId: string }>();

	const [selectedSeriesId, setSelectedSeriesId] = useState<string>();
	const [showEditSeriesModal, setShowEditSeriesModal] =
		useState<boolean>(false);
	const [showMarkReadModal, setShowMarkReadModal] = useState<boolean>(false);

	const edit = (id: string) => {
		setSelectedSeriesId(id);
		setShowEditSeriesModal(true);
	};

	const markReadClick = (id: string) => {
		setSelectedSeriesId(id);
		setShowMarkReadModal(true);
	};

	const markRead = () => {
		setShowMarkReadModal(false);
		setSelectedSeriesId(undefined);
	};

	return (
		<>
			{items &&
				items.map((item) => {
					return (
						<Figure key={item.id} className={styles.series}>
							<div className={styles.coverContainer}>
								<Figure.Image
									className={cn(styles.coverImage, "shadow")}
									src={`/api/media/${encodeURIComponent(
										item?.media?.coverSm || ""
									)}`}
								/>

								<div className={styles.overlay}>
									<Link
										key={item.id}
										to={`/library/${libraryId}/series/${item.id}`}>
										<SeriesCoverOverlay
											edit={() => edit(item.id)}
											markRead={() =>
												markReadClick(item.id)
											}
										/>
									</Link>
								</div>
							</div>

							<Figure.Caption className={styles.caption}>
								<Link
									className={styles.link}
									key={item.id}
									to={`/library/${libraryId}/series/${item.id}`}>
									<div className={styles.title}>
										{item.name}
									</div>
								</Link>
								<div className={styles.subtitle}>
									Volume {item.volume}
								</div>
								<div className={styles.subtitle}>
									{item.bookCount} Issues
								</div>
							</Figure.Caption>
						</Figure>
					);
				})}
		</>
	);
};
