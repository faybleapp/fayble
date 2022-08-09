import cn from "classnames";
import { useState } from "react";
import { Image as RBImage, Placeholder } from "react-bootstrap";
import styles from "./Image.module.scss";

interface ImageProps {
  src: string;
  className?: string;
  lazyLoad?: boolean;
}

export const Image = ({ src, className, lazyLoad }: ImageProps) => {
  const [loaded, setLoaded] = useState(false);

  return (
    <div className={cn(styles.imageContainer, className)}>
      <Placeholder
        animation="wave"
        className={cn(styles.placeholder, styles.image, {
          [styles.hidden]: loaded,
        })}
      />
      <RBImage
        loading={lazyLoad ? "lazy" : "eager"}
        className={cn(styles.image, { [styles.hidden]: !loaded })}
        src={src}
        onLoad={() => {
          setLoaded(true);
        }}
      />
    </div>
  );
};
