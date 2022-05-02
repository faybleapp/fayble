import { faStar as faStarEmpty } from "@fortawesome/free-regular-svg-icons";
import { faStar as faStarFull } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";
import React, { useState } from "react";
import styles from "./StarRating.module.scss";

interface StarRatingProps {
	rating?: number;
	onChange: (rating: number) => void;
}

export const StarRating = ({ rating, onChange }: StarRatingProps) => {
	const [hoverRating, setHoverRating] = useState<number | null>(null);

	const getIcon = (starNumber: number) => {
		let ratingValue = hoverRating !== null ? hoverRating : rating;

		if (!ratingValue) {
			return faStarEmpty;
		}
		if (ratingValue < starNumber) {
			return faStarEmpty;
		}
		return faStarFull;
	};

	const renderStars = () => {
		let stars = [];
		for (let i = 0; i <= 5; i++) {
			stars.push(
				<span
					onMouseOver={() => setHoverRating(i)}
					onMouseLeave={() => setHoverRating(null)}
					onClick={() => onChange(i)}>
					<FontAwesomeIcon
						icon={getIcon(i)}
						className={cn({ [styles.hiddenIcon]: i === 0 })}
					/>
				</span>
			);
		}
		return stars;
	};

	return <div>{renderStars()}</div>;
};
