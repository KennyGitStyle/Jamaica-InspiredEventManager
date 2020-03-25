import React from 'react';
import { Segment, Item, Header, Button, Image } from 'semantic-ui-react';
import { IEventActivity } from '../../../app/models/event-activity';
import { observer } from 'mobx-react-lite';
import { Link } from 'react-router-dom';
import { format } from 'date-fns';

const eventImageStyle = {
    filter: 'brightness(30%)'
};

const eventImageTextStyle = {
    position: 'absolute',
    bottom: '5%',
    left: '5%',
    width: '100%',
    height: 'auto',
    color: 'white'
};

const EventActivityDetailedHeader: React.FC<{ event: IEventActivity }> = ({ event }) => {
    return (

        <Segment.Group>
            <Segment basic attached='top' style={{ padding: '0' }}>
                <Image src={`/assets/categoryImages/${event.category}.jpg`} fluid style={eventImageStyle} />
                <Segment basic style={eventImageTextStyle}>
                    <Item.Group>
                        <Item>
                            <Item.Content>
                                <Header
                                    size='huge'
                                    content={event.title}
                                    style={{ color: 'white' }}
                                />
                                <p>{format(event.date, 'eeee do MMMM')}</p>
                                <p>
                                    Hosted by <strong>Bob</strong>
                                </p>
                            </Item.Content>
                        </Item>
                    </Item.Group>
                </Segment>
            </Segment>
            <Segment clearing attached='bottom'>
                <Button color='green'>Join Activity</Button>
                <Button>Cancel attendance</Button>
                <Button as={Link} to={`/manage/${event.id}`} color='yellow' floated='right'>
                    Manage Event
                    </Button>
            </Segment>
        </Segment.Group>

    )
}

export default observer(EventActivityDetailedHeader);
