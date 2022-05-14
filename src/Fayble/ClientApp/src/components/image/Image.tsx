import cn from "classnames";
import { Image as RBImage, Placeholder } from "react-bootstrap";
import styles from "./Image.module.scss";

interface ImageProps {
	src: string;
	className?: string;
	lazyLoad?: boolean;
}

export const Image = ({ src, className, lazyLoad }: ImageProps) => {
	return (
		<div className={cn(styles.imageContainer, className)}>
			<Placeholder
				animation="wave"
				className={cn(styles.placeholder, styles.image)}
			/>
			<RBImage
				loading={lazyLoad ? "lazy" : "eager"}
				className={styles.image}
				src={src}
			/>
		</div>
	);
};
