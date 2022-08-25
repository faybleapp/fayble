import { faBars, faTrashCan } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";

import { Image } from "components/image";
import { Book, Series } from "models/api-models";
import { ImageType } from "models/ui-models";
import { useState } from "react";
import { DropdownButton } from "react-bootstrap";
import { Link } from "react-router-dom";
import styles from "./CoverItem.module.scss";
interface CoverItemProps {
  item: Book | Series;
  title: string;
  firstSubtitle?: string;
  secondSubtitle?: string;
  link: string;
  isDeleted?: boolean;
  lazyLoad?: boolean;
  menuItems?: React.ReactNode;
}

export const CoverItem = ({
  item,
  title,
  firstSubtitle,
  secondSubtitle,
  link,
  lazyLoad = false,
  isDeleted = false,
  menuItems,
}: CoverItemProps) => {
  const [hovered, setHovered] = useState<boolean>(false);

  const handleHover = (hovered: boolean) => {
    setHovered(hovered);
  };

  return (
    <div key={item.id} className={styles.item}>
      <div
        className={styles.coverContainer}
        onMouseOver={() => handleHover(true)}
        onMouseLeave={() => handleHover(false)}>
        <Link key={item.id} to={link}>
          <Image
            className={cn(styles.coverImage, "shadow", {
              [styles.dimmed]: hovered || isDeleted,
            })}
            lazyLoad={lazyLoad}
            src={`/api/media?id=${item?.id}&mediaRoot=${item?.mediaRoot}&fileName=${ImageType.CoverSm}`}
          />
        </Link>
        {isDeleted ? (
          <div className={styles.deletedIcon}>
            <FontAwesomeIcon icon={faTrashCan} size="lg" />
          </div>
        ) : null}

        {menuItems && (
          <DropdownButton
            className={cn(styles.menuDropDown, {
              [styles.visible]: hovered,
            })}
            title={<FontAwesomeIcon icon={faBars} />}>
            {menuItems}
          </DropdownButton>
        )}
      </div>
      <div className={styles.caption}>
        <Link className={styles.link} key={item.id} to={link}>
          <div className={styles.title}>{title}</div>
        </Link>
        {firstSubtitle && (
          <div className={styles.subtitle}>{firstSubtitle}</div>
        )}
        {secondSubtitle && (
          <div className={styles.subtitle}>{secondSubtitle}</div>
        )}
      </div>
    </div>
  );
};
