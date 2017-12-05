<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<title>Led</title>
</head>
<body>
<?php

$state = false;

define("COUNT", 64);

$ledState = array();
for($idx=0;  $idx<COUNT; $idx++)
{
	$ledState[$idx] = "0";
}

$idx = '0';
$field = 'text' .strval($idx);
while(isset($_POST[$field]))
{
	$state = true;
	$param = $_POST[$field];
	$length = mb_strlen($param);
	if(strpos($param, 'time') !== false)
	{
		$data = explode(",", $param);
		$sec = intval($data[1]);
		sleep($sec);
	}
	else if(strpos($param, 'all') !== false)
	{
		for($j=0; $j<COUNT; $j++)
		{
			main();
		}
	}
	else if($length == 1)
	{
		//$data = explode(",", $param);
		//excute($data);
	}
	else if($length > 128-1)
	{
		$data = explode(",", $param);
		excute($data);
	}

	$idx++;
	$field = 'text' .strval($idx);
	sleep(0.1);
}

/*
while(isset($_POST['text']))
{
	echo "<p>loop:$text</p>";
	$param = $_POST['text'];
	switch(gettype($param))
	{
		case integer:
					break;
		case string:
					excute($param);
					break;
		default:
					break;
	}
	$idx++;
}
*/
/*
if(isset($POST))
{
	$text = "0,1,-1,-1";
	$data = explode(",", $text);
	$orders = array();

	for($i=0; $i<count($data)/4; $i++)
	{
		$order = new Order();
		$order->SetLabel($data[$i]);
		$order->SetType($data[$i+1]);
		$order->SetParam($data[$i+2], $data[$i+3]);
		$orders[] = $order;
	}

	$pos = 0;
	while(count($orders) >= $pos)
	{
			for($i=0; $i<count($orders); $i++)
			{
				if ($orders[$i]->getLabel() == $pos)
				{
					print("test");
					break;
				}
			}
			switch($orders[$i]->getType())
			{
				case 1: 
						excute(all(1));
						$pos += 1;
						break;
				case 2:

						excute(all(0));
						$pos += 1;
						break;
				default:
						break;
			}
	}
}
*/
function all($value)
{
	for($idx=0;  $idx<COUNT; $idx++)
	{
		$ledState[$idx] = $value;
	}
	return $ledState;
}



class Order{
	private $label;
	private $type;
	private $param = array(0, 0);
	
	function setLabel($label)
	{
		$this->label = $label;
	}
	function getLabel()
	{
		return $this->label;
	}
	
	function setType($type)
	{
		$this->type = $type;
	}
	function getType()
	{
		return $this->type;
	}

	function setParam($a, $b)
	{
		$this->param[0] = $a;
		$this->param[1] = $b;
	}
}

function update()
{
	for($idx=0;  $idx<COUNT; $idx++)
	{
		if ($ledState[$idx] == 0)
		{
			$ledState[$idx] = 1;
			break;
		}
	}
	return $ledState;
}

function excute($data)
{
	$param = "";
	for($idx=0;  $idx<64; $idx++)
	{
		$param .= $data[$idx].",";
	}
	$fullPath='python /var/www/html/ledClient.py '.$param;//.' > /dev/null &';
	exec($fullPath, $outpara);
	var_dump($outpara[0]);//var_dump($outpara[1]);var_dump($outpara[2]);var_dump($outpara[3]);
}

function main()
{
	global $ledState;
	if($ledState[COUNT-1] == "0" )
	{
		for($idx=0;  $idx<COUNT; $idx++)
		{
			if ($ledState[$idx] == "0")
			{
				$ledState[$idx] = "1";
				break;
			}
		}
	}
	else
	{
		for($idx=0;  $idx<COUNT; $idx++)
		{
			if ($ledState[$idx] == "1")
			{
				$ledState[$idx] = "0";
				break;
			}
		}
	}
	$param = "";
	for($idx=0;  $idx<64; $idx++)
	{
		$param .= $ledState[$idx].",";
	}
	excute($ledState);
}

?>
</body>
</html>
