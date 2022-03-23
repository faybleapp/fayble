import { faStar as faStarO } from "@fortawesome/free-regular-svg-icons";
import { faStar } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Series } from "models/api-models";
import React, { useEffect, useState } from "react";
import { Image } from "react-bootstrap";
import Rating from "react-rating";
import ShowMoreText from "react-show-more-text";
import { useUpdateSeries } from "services";
import styles from "./SeriesDetail.module.scss";

interface SeriesDetailProps {
	series: Series;
}

export const SeriesDetail = (props: SeriesDetailProps) => {
	const [series, setSeries] = useState<Series>(props.series);

	useEffect(() => {
		setSeries(props.series);
	}, [props.series]);

	const updateSeries = useUpdateSeries();

	const ratingChanged = (rating: number) => {
		const updatedSeries = { ...series, rating: rating };
		updateSeries.mutate([series.id, updatedSeries]);
		setSeries(updatedSeries);
	};

	return (
		<div className={styles.container}>
			<Image
				className={styles.cover}
				src={
					series &&
					`/api/media/${encodeURIComponent(series.media?.coverSm!)}`
				}
			/>
			<div className={styles.detailsPanel}>
				<div className={styles.detailsTitle}>
					<h4>{series?.name}</h4>

					<div className={styles.rating}>
						<Rating
							className={styles.rating}
							fractions={2}
							onChange={ratingChanged}
							initialRating={series?.rating}
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
				{series?.year && (
					<div className={styles.detailProperty}>
						<div className={styles.detailsHeading}>Year</div>
						<div>{series?.year}</div>
					</div>
				)}

				<div className={styles.detailProperty}>
					<div className={styles.detailsHeading}>Issues</div>
					<div className={styles.detailsValue}>
						{series?.bookCount}
					</div>
				</div>

				{series?.volume && (
					<div className={styles.detailProperty}>
						<div className={styles.detailsHeading}>Volume</div>
						<div>{series?.volume}</div>
					</div>
				)}

				{series?.publisher && (
					<div className={styles.detailProperty}>
						<div className={styles.detailsHeading}>Publisher</div>
						<div>{series?.publisher?.name}</div>
					</div>
				)}

				<div className={styles.summary}>
					<ShowMoreText
						lines={3}
						more="show more"
						less="show less"
						anchorClass=""
						width={750}>
						{series?.summary}
					</ShowMoreText>
				</div>
			</div>
		</div>
	);
};
