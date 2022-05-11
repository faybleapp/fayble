import { StarRating } from "components/starRating";
import { Series } from "models/api-models";
import { ImageTypes } from "models/ui-models";
import { useEffect, useState } from "react";
import { Image } from "react-bootstrap";
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
				src={`/api/media?id=${series?.id}&mediaRoot=${series?.mediaRoot}&fileName=${ImageTypes.CoverSm}`}
			/>
			<div className={styles.detailsPanel}>
				<div className={styles.detailsTitle}>
					<h4>{series?.name}</h4>

					<div className={styles.rating}>
						<StarRating rating={series.rating} onChange={ratingChanged}/>
					</div>
				</div>
				<hr />
				<div className={styles.detailSection}>
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
							<div className={styles.detailsHeading}>
								Publisher
							</div>
							<div>{series?.publisher?.name}</div>
						</div>
					)}
				</div>
				<div className={styles.summary}>{series?.summary}</div>
			</div>
		</div>
	);
};
