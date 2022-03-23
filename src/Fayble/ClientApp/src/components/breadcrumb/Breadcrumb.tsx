import { faHome } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { BreadcrumbItem } from "models/ui-models";
import { Breadcrumb as BCrumb } from "react-bootstrap";
import { Link } from "react-router-dom";
import styles from "./Breadcrumb.module.scss";

interface BreadcrumbProps {
	items: BreadcrumbItem[];
}

export const Breadcrumb = ({ items }: BreadcrumbProps) => {
	return (
		<div className={styles.container}>
			<BCrumb>
				{items &&
					items.map((item, index) => {
						return (
							<BCrumb.Item
								className={styles.breadcrumbItem}
								key={index}
								linkAs={Link}
								linkProps={{ to: item.link }}>
								{index === 0 ? (
									<FontAwesomeIcon
										className={styles.homeIcon}
										icon={faHome}
									/>
								) : null}
								{item.name}
							</BCrumb.Item>
						);
					})}
			</BCrumb>
		</div>
	);
};
